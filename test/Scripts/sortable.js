var sortableApp = angular.module("sortable", []);

sortableApp.controller('SortableController', function SortableController($scope, $http)
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
