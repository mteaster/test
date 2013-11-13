var sortableApp = angular.module("sortable", []);

sortableApp.controller('SortableController', function FilesController($scope, $http)
{
    console.log("running controller");

    $scope.data = [];
    $scope.init = function (url)
    {
        console.log(url);
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

        console.log("sorting by column " + $scope.column + ", " + ($scope.descending) ? "descending" : "ascending" );
    };
});