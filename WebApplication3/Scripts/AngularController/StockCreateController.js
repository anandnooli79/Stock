angular.module('MyApp') // extending angular module from first part
.controller('StockCreateController', function ($scope, FileUploadService) {
    // Variables
    $scope.Message = "";
    $scope.FileInvalidMessage = "";
    $scope.UploadedStocks = [];
    //Form Validation
    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    // THIS IS REQUIRED AS File Control is not supported 2 way binding features of Angular
    // ------------------------------------------------------------------------------------
    //File Validation
    $scope.ChechFileValid = function (file) {
        var isValid = false;
        if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024 * 1024)) {
            isValid = true;
        }
        else {
            $scope.FileInvalidMessage += file.name +" Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
        }
       return isValid;
    };
    
    //----------------------------------------------------------------------------------------
    $scope.StockFileforUpload = function(files)
    {
        $scope.FileInvalidMessage = "";
        angular.forEach(files, function (file, key) {
            if ($scope.ChechFileValid(file)) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if ($scope.UploadedStocks.length <= 0) {
                        var stock = { Id: 1, SKUId: "", StockName: "", StockDescription: "", StockImage: event.target.result, StockPrice: 0.0, DiscountPercent: 0, IsInStock: true ,IsUploading:false, Message:""};
                        $scope.UploadedStocks.push(stock);
                    }
                    else {
                        var stock = { Id: $scope.UploadedStocks[$scope.UploadedStocks.length - 1].Id + 1, SKUId: "", StockName: "", StockDescription: "", StockImage: event.target.result, StockPrice: 0.0, DiscountPercent: 0, IsInStock: true, IsUploading: false, Message: "" };
                        $scope.UploadedStocks.push(stock);
                    }
                }
                reader.readAsDataURL(file);
            }
        });
    }
    //Save File
    $scope.SaveFile = function (UploadedStock) {
        if (UploadedStock.SKUId == "" || UploadedStock.StockName == "") {
            UploadedStock.Message = "All the fields are required.";
        }
        else {
            FileUploadService.UploadFile(UploadedStock).then(function (d) {
                UploadedStock.IsUploading = false;
            }, function (e) {
                UploadedStock.Message = "Failed to add stock!"
            });
        }
    };
})
.factory('FileUploadService', function ($http, $q) { // explained abour controller and service in part 2

    var fac = {};
    fac.UploadFile = function (UploadedStock) {
        var defer = $q.defer();
        UploadedStock.IsUploading = true;
        var formData = new FormData()
        formData.append("skuId", UploadedStock.SKUId);
        formData.append("discountPrecent", UploadedStock.DiscountPercent);
        formData.append("stockDescription", UploadedStock.StockDescription);
        formData.append("stockImage", UploadedStock.StockImage);
        formData.append("isInStock", UploadedStock.IsInStock);
        formData.append("stockName", UploadedStock.StockName);
        formData.append("stockPrice", UploadedStock.StockPrice);
        var defer = $q.defer();
        $http.post("/Data/UploadStock", formData, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        })
        .then(function (d) {
            defer.resolve(d);
           
        },function () {
            defer.reject("File Upload Failed!");
        });

        return defer.promise;

    }
    return fac;

});