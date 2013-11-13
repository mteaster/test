var filesApp = angular.module("files", []);

filesApp.controller('FilesController', function FilesController($scope, $http)
{
    console.log("running controller");

    $scope.init = function (url)
    {
        $scope.url = url;
    };

    var url = '/FileCabinet/GetJson?bandId=' + bandId + '&groupId=' + groupId;

    $scope.data = [];
    $http({ method: 'GET', url: url }).success(function (data) { $scope.files = data; });

    $scope.column = '';
    $scope.descending = false;

    $scope.sort = function (column)
    {
        console.log("sorting");

        if ($scope.column == column)
        {
            $scope.descending = !$scope.descending;
        }
        else
        {
            $scope.column = column;
            $scope.descending = false;
        }

        console.log("sorting by column " + $scope.column);
        console.log("descending = " + $scope.descending);
    };
});