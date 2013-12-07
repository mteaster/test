var sortableApp = angular.module("sortable", []);

sortableApp.controller('SortableController', function FilesController($scope, $http)
{
    $scope.data = [];
    $scope.url = "";
    $scope.criteria = "";

    $scope.init = function (url)
    {
        $scope.url = url;
    };

    $scope.search = function ()
    {
        console.log($scope.criteria);
        $http({ method: 'GET', url: $scope.url + '/' + $scope.criteria }).success(function (data) { $scope.data = data; });
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