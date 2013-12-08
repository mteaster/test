var superApp = angular.module("super", []);

superApp.controller('SortableController', function SortableController($scope, $http)
{
    $scope.data = [];

    $scope.init = function (url)
    {
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

superApp.controller('SearchableController', function SearchableController($scope, $http)
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