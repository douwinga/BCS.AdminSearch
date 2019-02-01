var vueMultiselect = Vue.component('vue-multiselect', window.VueMultiselect.default);
var searchFiltersJson = JSON.parse(searchFilters);
var activeFiltersJson = JSON.parse(activeFilters);
var data = {
    filtersCollapsed: true,
    selectedFilter: searchFiltersJson.length ? searchFiltersJson[0] : {},
    searchFilters: searchFiltersJson,
    activeFilters: {},
    values: {},
    searchTerm: searchTerm
};
searchFiltersJson.forEach(function (filter) {
    data.values[filter.name] = [];
});
activeFiltersJson.forEach(function (filter) {
    filter.value.forEach(function (value) {
        var found = [];
        searchFiltersJson.forEach(function (obj) {
            found = obj.options.filter(function (item) { return item.value === value; });
            if (found.length) {
                found[0].parent = obj.name;
                found[0].indexField = obj.indexField;
                !data.activeFilters[obj.name] ? data.activeFilters[obj.name] = [found[0]] : data.activeFilters[obj.name].push(found[0]);
                !data.values[obj.name] ? data.values[obj.name] = [found[0]] : data.values[obj.name].push(found[0]);
                return;
            }
        });
    });
});
var vueSearch = new Vue({
    el: '#vueSearch',
    components: { 'vue-multiselect': vueMultiselect },
    data: data,
    created: function () {
        if (Object.keys(this.activeFilters).length) {
            this.filtersCollapsed = false;
        }
    },
    methods: {
        onSelect: function (value) {
            value.parent = this.selectedFilter.name;
            value.indexField = this.selectedFilter.indexField;

            if (!this.activeFilters[value.parent]) {
                this.activeFilters[value.parent] = [value];
            } else {
                this.activeFilters[value.parent].push(value);
            }
        },
        onRemove: function (value) {
            if (!!value.parent && !!this.activeFilters[value.parent]) {
                var activeFilterIndex = this.activeFilters[value.parent].findIndex(function (obj) { return obj.name === value.name; });
                this.activeFilters[value.parent].splice(activeFilterIndex, 1);
                if (!this.activeFilters[value.parent].length) {
                    delete this.activeFilters[value.parent];
                }
                this.$forceUpdate();
            }
        },
        onRemoveTag: function (name, parent) {
            var value = {
                name: name,
                parent: parent
            };

            this.onRemove(value);

            if (!!value.parent && !!this.values[value.parent]) {
                var valueIndex = this.values[value.parent].findIndex(function (obj) { return obj.name === value.name; });
                this.values[value.parent].splice(valueIndex, 1);
            }
        },
        onFilterChange: function (event) {
            this.selectedFilter = filtersJson[event.target.selectedIndex];
        },
        applyFilters: function (event) {
            event.preventDefault();
            var filterArray = [];
            for (var group in this.activeFilters) {
                this.activeFilters[group].forEach(function (filter) {
                    if (!filterArray[filter.indexField]) {
                        filterArray[filter.indexField] = [filter.value];
                    } else {
                        filterArray[filter.indexField].push(filter.value);
                    }
                });
            }

            window.location.search = this.serialize(filterArray);
        },
        serialize: function (filters) {
            var queryParams = new URLSearchParams(window.location.search);
            queryParams.set("searchTerm", data.searchTerm);

            // Clear previous filters
            var previousFilters = Array.from(queryParams.keys()).filter(function (k) {
                return k.startsWith('filters[');
            });
            previousFilters.forEach(function (filter) {
                queryParams.delete(filter);
            });

            var index = 0;
            for (var p in filters) {
                if (filters.hasOwnProperty(p)) {
                    queryParams.set("filters[" + index + "].Key", encodeURIComponent(p));

                    filters[p].forEach(function (value) {
                        queryParams.append("filters[" + index + "].Value", encodeURIComponent(value));
                    });

                    index++;
                }
            }

            return queryParams.toString();
        }
    }
});
