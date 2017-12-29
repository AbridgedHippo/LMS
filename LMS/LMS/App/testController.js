(function () {

    var app = angular.module("LMSApp").controller("testController", ["$scope", "$http", "$window", "FileHandler", function ($scope, $http, $window, FileHandler) {

        var vm = this;

        $scope.getFileDetails = function (e) {

            var files = [];
            for (var i = 0; i < e.files.length; i++) {
                files.push(e.files[i])
            }
            FileHandler.setFileDetails(files);

        };

        vm.upload = function () {
            FileHandler.uploadFile();
        }

    }]);

})();