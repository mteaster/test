var sortableApp = angular.module("sortable", []);

sortableApp.controller('SortableController', function FilesController($scope, $http)
{
    $scope.loaded = false;
    $scope.data = [];

    $scope.init = function (url)
    {
        $scope.loaded = true;
        $http({ method: 'GET', url: url }).success(function (data) { $scope.data = data; });
    };

    $scope.column = '';
    $scope.descending = false;

    $scope.sort = function (column)
    {
        if ($scope.column == column)
        {
            $scope.descending = !$scope.descending;
        }
        else
        {
            $scope.column = column;
            $scope.descending = false;
        }
    };
});