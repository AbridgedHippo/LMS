(function () {

    angular.module("LMSApp").factory("FileHandler", function ($http, $window, Helpers) {

        return {
            uploadFile: uploadFile,
            downloadFile: downloadFile,
            setFileDetails: setFileDetails
        };

        var files = [];

        function setFileDetails(newfiles) {

            // Prep files for upload
            files = newfiles;
        };

        function uploadFile() {

            //FILL FormData WITH FILE DETAILS.
            var data = new FormData();

            for (var i in files) {
                data.append("uploadedFile", files[i]);
            }

            Helpers.postData("/api/files/", data);

            // ADD LISTENERS.
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("load", transferComplete, false);

            // SEND FILE DETAILS TO THE API.
            objXhr.open("POST", "/api/files/");
            //objXhr.setRequestHeader('Authorization', 'Token token="' + $window.sessionStorage.getItem("tokenKey") + '"');
            console.log($window.sessionStorage.getItem("tokenKey"));
            objXhr.send(data);



        }

        // debug? confirmation.
        function transferComplete(e) {
            alert("Files uploaded successfully.");
        }

        function downloadFile() {

        }

        //Example code for upload:
        //html:
        //<input type="file" id="file" name="file" multiple onchange="angular.element(this).scope().getFileDetails(this)" />
        //<input type="button" ng-click="vm.upload()" value="Upload" />

        //angular:
        //var vm = this;
        //
        //$scope.getFileDetails = function (e) {
        //
        //    var files = [];
        //    for (var i = 0; i < e.files.length; i++) {
        //        files.push(e.files[i])
        //    }
        //    fileHandler.setFileDetails(files);
        //
        //};
        //
        //vm.upload = function () {
        //    fileHandler.uploadFile();
        //}

    });

})();