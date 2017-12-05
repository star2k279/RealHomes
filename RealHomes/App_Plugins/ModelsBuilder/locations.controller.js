angular.module("umbraco")
    .controller("RealHomes.LocationsController",
    function ($scope) {

        $scope.makes = [
            { id: "key1", label: "val1" },
            { id: "key2", label: "val2" },
            { id: "key3", label: "val3" }
        ];

        $scope.saveKeyValue = function () {
            $scope.model.value = { key: $scope.model.value, value: $scope.makes[$scope.model.value] }

            console.log($scope.model.value);
        };
    });