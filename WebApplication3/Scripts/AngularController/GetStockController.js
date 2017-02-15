angular.module('MyApp') // extending angular module from first part
.controller('GetStockController', function ($scope, GetFileService) {
    // Variables
    $scope.Message = "";
    $scope.image = {
        path: "",
        width: 600,
        height: 600
    }

    $scope.currentFiles = [];
    GetFileService.GetFiles().then(function (d) {
        d.data.UploadedFiles.forEach(function (uploadFileEntry) {
            $scope.currentFiles.push(uploadFileEntry);
        }, function (e) { alert(e); });
    })
})
.factory('GetFileService', function ($http, $q) {

    var fac = {};
    fac.GetFiles = function () {
        var defer = $q.defer();
        $http.get("/Data/GetFiles",
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .then(function (d) {
            defer.resolve(d);

        }, function () {
            defer.reject("File Upload Failed!");
        });

        return defer.promise;

    }
    return fac;

});