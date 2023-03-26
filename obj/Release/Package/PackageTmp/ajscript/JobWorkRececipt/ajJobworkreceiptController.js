myApp.controller("JobWorkReceiptCtrl", function ($scope, $http, JobWorkReceiptCtrlService) {
    $scope.test = function () {
        var test = JobWorkReceiptCtrlService.test();
        test.then(function (response) {
           // alert(1);
        });
        console.log($scope);
        $scope.$digest();
        

        console.log($scope);

    };
    
});