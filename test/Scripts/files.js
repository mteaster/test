var filesApp = angular.module("files", []);

filesApp.controller('FilesController', function FilesController($scope, $http)
{
    console.log("running controller");

    $http({ method: 'GET', url: '/FileCabinet/GetJson?groupId=1' }).
        success(function (data, status, headers, config)
        {
            console.log("success");
            console.log(data);
        }).
        error(function (data, status, headers, config)
        {
            console.log("error");
            console.log(data);
        });
});