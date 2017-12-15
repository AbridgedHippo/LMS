(function () {

    var app = angular.module("AdminApp").controller("AdminController", ["Helpers", "BootstrappedData", "$scope", "$uibModal", function (Helpers, BootstrappedData, $scope, $uibModal) {
        "use strict";

        var $ctrl = this;

        $ctrl.newUser = {
            FirstName: "Adam",
            LastName: "Jonsson",
            Role: "Student"
        };
        $ctrl.newRole = {
            Name: "Student"
        };

        $ctrl.init = function () {
            $ctrl.users = BootstrappedData.data.Users;
            $ctrl.roles = BootstrappedData.data.Roles;
        };

        $ctrl.createDialog = function () {
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CreateUserModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    createUser();
                }, function () {
                });
        };
        $ctrl.editDialog = function (user) {
            $ctrl.editUser = copyUser(user);
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/EditUserModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    editUser();
                }, function () {
                    $ctrl.editUser = null;
                });
        };
        $ctrl.deleteDialog = function (user) {
            $ctrl.deleteUser = user;
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/DeleteUsermodal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    deleteUser();
                }, function () {
                    $ctrl.deleteUser = null;
                });
        };

        // users
        $ctrl.users = [];
        var getUsers = function () {
            Helpers.postData("/api/admin/getusers")
                .then(function (response) {
                    $ctrl.users = response.data;
                });
        };
        var createUser = function () {
            Helpers.postData("/api/admin/createuser", $ctrl.newUser)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    getUsers();
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        var deleteUser = function () {
            Helpers.postData("/api/admin/DeleteUser", $ctrl.deleteUser)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    getUsers();
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        var editUser = function () {
            Helpers.postData("/api/admin/EditUser", $ctrl.editUser)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    getUsers();
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };

        // roles
        $ctrl.roles = [];
        var getRoles = function () {
            Helpers.postData("/api/admin/getroles")
                .then(function (response) {
                    $ctrl.roles = response.data;
                });
        };
        $ctrl.createRole = function () {
            Helpers.postData("/api/admin/createrole", $ctrl.newRole)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    getRoles();
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        $ctrl.deleteRole = function (role) {
            Helpers.postData("/api/admin/DeleteRole", role)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    getRoles();
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        $ctrl.editRole = function (role) {
            alert(role);
        };

        var copyUser = function (user) {
            return {
                Id: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Role: user.Role
            };
        };

    }]);
})();