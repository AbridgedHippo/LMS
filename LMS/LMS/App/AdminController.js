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
        $ctrl.newCourse = {
            Id: "M0043M",
            Name: "Matematik II"
        };
        $ctrl.newAssignment = {
            Id: +0,
            Name: "Assignment 1",
            Description: "The first Assignment",
            CourseId: +0,
            Course: ""
        };

        // helpers
        var updateUsers = function () {
            repo.updateUsers()
                .then(function (response) {
                    $ctrl.users = response.data;
                    $ctrl.teachers = [];
                    $ctrl.students = [];
                    for (var i = 0, j = $ctrl.users.length; i < j; i++) {
                        var user = $ctrl.users[i];
                        if (user.Role === "Teacher") {
                            $ctrl.teachers.push(user);
                        }
                        else if (user.Role === "Student") {
                            $ctrl.students.push(user);
                        }
                    }
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });

        };
        var updateRoles = function () {
            repo.updateRoles()
                .then(function (response) {
                    $ctrl.roles = response;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        var updateCourses = function () {
            repo.updateCourses()
                .then(function (response) {
                    $ctrl.courses = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        var checkUserRole = function (user) {
            if (!user.Role) {
                for (var i = 0, j = $ctrl.users.length; i < j; i++) {
                    if ($ctrl.users[i].UserName === user.UserName) {
                        return $ctrl.users[i];
                    }
                }
            }
            return user;
        };


        $ctrl.init = function () {
            $ctrl.users = repo.initUsers();
            $ctrl.teachers = [];
            $ctrl.students = [];
            for (var i = 0, j = $ctrl.users.length; i < j; i++) {
                var user = $ctrl.users[i];
                if (user.Role === "Teacher") {
                    $ctrl.teachers.push(user);
                    if (!$ctrl.addTeacher) {
                        $ctrl.addTeacher = user;
                    }
                }
                else if (user.Role === "Student") {
                    $ctrl.students.push(user);
                }
            }
            $ctrl.roles = repo.initRoles();
            $ctrl.courses = repo.initCourses();
        };
        $ctrl.toggleEdit = function () {
            var elements = document.getElementsByClassName("disableable");
            for (var i = 0, j = elements.length; i < j; i++) {

                if (!elements[i].disabled) {
                    elements[i].disabled = true;
                }
                else {
                    elements[i].disabled = false;
                }
            }
        };

        // course Assignments
        $ctrl.assignmentInCourse = function (assignment) {
            if ($ctrl.assignments) {
                var assignments = $ctrl.assignments;
                for (var i = 0, j = assignments.length; i < j; i++) {
                    if (assignments[i].CourseId === assignment.CourseId) {
                        return true;
                    }
                }
            }
            return false;
        };
        $ctrl.courseAssignmentsDialog = function (course) {
            repo.getAssignmentsForCourse(course)
                .then(function (response) {
                    $ctrl.assignments = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CreateCourseAssignmentModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    $ctrl.createAssignmentForCourse(course);
                }, function () {
                });
            $ctrl.assignments = null;
        };
        $ctrl.createAssignmentForCourse = function (course) {
            $ctrl.newAssignment.CourseId = course.Id;
            repo.createAssignmentForCourse($ctrl.newAssignment)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    repo.getAssignmentsForCourse($ctrl.editCourse)
                        .then(function (response) {
                            $ctrl.assignments = response.data;
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        $ctrl.deleteAssignmentFromCourse = function (assignment) {
            repo.deleteAssignmentFromCourse(assignment)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    repo.getAssignmentsForCourse($ctrl.editCourse)
                        .then(function (response) {
                            $ctrl.assignments = response.data;
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        $ctrl.editAssignmentDialog = function (assignment) {
            repo.getAssignment(assignment)
                .then(function (response) {
                    $ctrl.editAssignment = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/EditAssignmentModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    console.log(assignment);
                    console.log($ctrl.editAssignment);
                    repo.editAssignmentForCourse($ctrl.editAssignment)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                }, function () {
                });
            $ctrl.editAssignment = null;
        };
        
        // Course Users
        $ctrl.courseTeachersDialog = function (course) {
            repo.getCourse(course)
                .then(function (response) {
                    $ctrl.editCourse = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CourseTeachersModal.cshtml",
                scope: $scope
            })
                .result.then(function () { }, function () { });
            $ctrl.editCourse = null;
            $ctrl.courseTeachers = [];
        };
        $ctrl.courseStudentsDialog = function (course) {
            repo.getCourse(course)
                .then(function (response) {
                    $ctrl.editCourse = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CourseStudentsModal.cshtml",
                scope: $scope
            })
                .result.then(function () { }, function () { });
            $ctrl.editCourse = null;
            $ctrl.courseTeachers = [];
        };
        $ctrl.addUserToCourse = function (user) {
            repo.addUserToCourse(user, $ctrl.editCourse)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    if (user.Role === "Teacher") {
                        repo.getTeachersForCourse($ctrl.editCourse)
                            .then(function (response) {
                                $ctrl.editCourse.Teachers = response.data;
                            }, function (error) {
                                $ctrl.statusMsg = error.data;
                            });
                    }
                    else if (user.Role === "Student") {
                        repo.getStudentsForCourse($ctrl.editCourse)
                            .then(function (response) {
                                $ctrl.editCourse.Students = response.data;
                            }, function (error) {
                                $ctrl.statusMsg = error.data;
                            });
                    }
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        $ctrl.removeUserFromCourse = function (user) {
            user = checkUserRole(user);
            repo.removeUserFromCourse(user, $ctrl.editCourse)
                .then(function (response) {
                    $ctrl.statusMsg = response.data;
                    if (user.Role === "Teacher") {
                        repo.getTeachersForCourse($ctrl.editCourse)
                            .then(function (response) {
                                $ctrl.editCourse.Teachers = response.data;
                            }, function (error) {
                                $ctrl.statusMsg = error.data;
                            });
                    }
                    else if (user.Role === "Student") {
                        repo.getStudentsForCourse($ctrl.editCourse)
                            .then(function (response) {
                                $ctrl.editCourse.Students = response.data;
                            }, function (error) {
                                $ctrl.statusMsg = error.data;
                            });
                    }
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
        };
        $ctrl.userInCourse = function (user) {
            if ($ctrl.editCourse) {
                if (user.Role === "Teacher") {
                    var teachers = $ctrl.editCourse.Teachers;
                    for (var i = 0, j = teachers.length; i < j; i++) {
                        if (teachers[i].UserName === user.UserName) {
                            return true;
                        }
                    }
                }
                else if (user.Role === "Student") {
                    var students = $ctrl.editCourse.Students;
                    for (var i = 0, j = students.length; i < j; i++) {
                        if (students[i].UserName === user.UserName) {
                            return true;
                        }
                    }
                }
            }
            return false;
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
            if (!user.Role) {
                var fromCourse = true;
                user = checkUserRole(user);
            }
            repo.getUser(user)
                .then(function (response) {
                    $ctrl.editUser = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/EditUserModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    $ctrl.editing = false;
                    repo.editUser($ctrl.editUser)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateUsers();
                            if (fromCourse) {
                                
                                if (user.Role === "Teacher") {
                                    repo.getTeachersForCourse($ctrl.editCourse)
                                        .then(function (response) {
                                            $ctrl.editCourse.Teachers = response.data;
                                        }, function (error) {
                                            $ctrl.statusMsg = error.data;
                                        });
                                }
                                else if (user.Role === "Student") {
                                    repo.getStudentsForCourse($ctrl.editCourse)
                                        .then(function (response) {
                                            $ctrl.editCourse.Students = response.data;
                                        }, function (error) {
                                            $ctrl.statusMsg = error.data;
                                        });
                                }

                            }
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.editUser = null;
                    $ctrl.editing = false;
                }, function () {
                });
        };
        $ctrl.deleteDialog = function (user) {
            $ctrl.deleteUser = user;
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/DeleteUserModal.cshtml",
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
        $ctrl.setPasswordDialog = function (user) {
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/SetUserPasswordModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    var data = {
                        User: user,
                        Passwords: $ctrl.passwords
                    };
                    repo.setPassword(data)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateUsers();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.passwords = {};
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
            $ctrl.editing = false;
            repo.getRole(role)
                .then(function (response) {
                    $ctrl.editRole = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/EditRoleModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.editRole($ctrl.editRole)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateRoles();
                            updateUsers();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
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

        // Course Modals
        $ctrl.createCourseDialog = function () {
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/CreateCourseModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.createCourse($ctrl.newCourse)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateCourses();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                }, function () {
                });
        };
        $ctrl.editCourseDialog = function (course) {
            repo.getCourse(course)
                .then(function (response) {
                    $ctrl.editCourse = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            repo.getAssignmentsForCourse(course)
                .then(function (response) {
                    $ctrl.assignments = response.data;
                }, function (error) {
                    $ctrl.statusMsg = error.data;
                });
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/EditCourseModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.editCourse($ctrl.editCourse)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateCourses();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.editCourse = null;
                    $ctrl.courseTeachers = [];
                }, function () {
                });
        };
        $ctrl.deleteCourseDialog = function (course) {
            $ctrl.deleteCourse = course;
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/DeleteCourseModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    repo.deleteCourse(course)
                        .then(function (response) {
                            $ctrl.statusMsg = response.data;
                            updateCourses();
                        }, function (error) {
                            $ctrl.statusMsg = error.data;
                        });
                    $ctrl.deleteCourse = null;
                }, function () {
                });
        };

        // 

    }]);
})();