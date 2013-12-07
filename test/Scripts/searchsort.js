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
        $http({ method: 'GET', url: url + '/' + criteria }).success(function (data) { $scope.data = data; });
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