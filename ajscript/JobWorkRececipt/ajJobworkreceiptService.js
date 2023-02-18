var myApp = angular.module('myApp', []);
myApp.service("JobWorkReceiptCtrlService", function ($http) {

    this.test = function () {
        var response = $http({
            method: "get",
            url: "/Home/Test"
        });

        return response;
    }
});