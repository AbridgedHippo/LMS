(function () {

    var app = angular.module("AdminApp").controller("AdminController", ["Helpers", "AdminRepository", "$scope", "$uibModal", function (Helpers, AdminRepository, $scope, $uibModal) {
        "use strict";

        var $ctrl = this;
        var repo = AdminRepository;

        $ctrl.newUser = {
            FirstName: "Per",
            LastName: "Persson",
            Role: "Student"
        };
        $ctrl.newRole = {
            Name: "Student"
        };

        $ctrl.init = function () {
            $ctrl.users = repo.initUsers();
            $ctrl.roles = repo.initRoles();
        };

        // User Modals
        $ctrl.createDialog = function () {
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CreateUserModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.createUser($ctrl.newUser)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateUsers();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
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
                    repo.editUser($ctrl.editUser)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateUsers();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.editUser = null;
                }, function () {
                });
        };
        $ctrl.deleteDialog = function (user) {
            $ctrl.deleteUser = user;
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/DeleteUsermodal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.deleteUser(user)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateUsers();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.deleteUser = null;
                }, function () {
                });
        };

        // Role Modals
        $ctrl.createRoleDialog = function () {
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CreateRoleModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.createRole($ctrl.newRole)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateRoles();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                }, function () {
                });
        };
        $ctrl.editRoleDialog = function (role) {
            $ctrl.editRole = copyRole(role);
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/EditRoleModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.editRole($ctrl.editRole)
                        //.then(function (response) {
                        //    $ctrl.statusMsg = response.data;
                        //    updateRoles();
                        //}, function (error) {
                        //    $ctrl.statusMsg = error.data;
                        //});
                    $ctrl.editRole = null;
                }, function () {
                });
        };
        $ctrl.deleteRoleDialog = function (role) {
            $ctrl.deleteRole = role;
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/DeleteRoleModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.deleteRole(role)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateRoles();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.deleteRole = null;
                }, function () {
                });
        };

        // helpers
        var updateUsers = function () {
            repo.updateUsers()
                .then(function (response) {
                    $ctrl.users = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });;
        };
        var updateRoles = function () {
            repo.updateRoles()
                .then(function (response) {
                    $ctrl.roles = response;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });;
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
        var copyRole = function (role) {
            return {
                Id: role.Id,
                Name: role.Name
            };
        };

    }]);
})();