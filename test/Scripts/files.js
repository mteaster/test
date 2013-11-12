var filesApp = angular.module("files", []);

filesApp.controller('FilesController', function FilesController($scope, $http)
{
    console.log("running controller");

    $scope.files = [];

    $scope.sort =
    {
        column: '',
        descending: false
    };

    $scope.changeSorting = function (column)
    {
        console.log("change sorting");
        var sort = $scope.sort;

        if (sort.column == column)
        {
            sort.descending = !sort.descending;
        }
        else
        {
            sort.column = column;
            sort.descending = false;
        }

        console.log($sort.column);
        console.log($sort.descending);
    };

    var filesUrl = '/FileCabinet/GetJson?bandId=' + bandId + '&groupId=' + groupId;
    $http({ method: 'GET', url: filesUrl }).
        success(function (data, status, headers, config)
        {
            console.log("success");
            console.log(data);
            $scope.files = data;
        }).
        error(function (data, status, headers, config)
        {
            console.log("error");
            console.log(data);
        });
});