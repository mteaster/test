var superApp = angular.module("super", []);

superApp.controller('SortableController', function SortableController($scope, $http)
{
    $scope.data = [];
    $scope.ready = false;
    $scope.empty = false;

    $scope.init = function (url)
    {
        $http({ method: 'GET', url: url }).success(function (data)
        {
            $scope.data = data;
            $scope.ready = true;
            $scope.empty = (data.length === 0) ? true : false;
        });

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

superApp.controller('SearchableController', function SearchableController($scope, $http)
{
    $scope.data = [];
    $scope.url = "";
    $scope.criteria = "";
    $scope.searched = false;

    $scope.init = function (url)
    {
        $scope.url = url;
    };

    $scope.search = function ()
    {
        $scope.searched = true;
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