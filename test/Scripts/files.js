var filesApp = angular.module("files", []);

filesApp.controller('FilesController', function FilesController($scope, $http)
{
    console.log("running controller");

    $sscope.files = [];

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