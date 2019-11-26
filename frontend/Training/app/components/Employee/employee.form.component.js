'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Employee
 * @description
 * # Employee
 */
angular.module('Training').directive('employeeForm', function() {
    return {
        templateUrl: 'components/Employee/employee.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, EmployeeService, ShiftService, LevelService, JobPositionService, SkillService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Employee',
                baseService: EmployeeService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {
                    $scope.onJobPositionChange();
                }
            });

            $scope.$on('load-modal-Employee', function(scope, oEmployee) {
                refresh(oEmployee);
            });

            $scope.$on('ok-modal-Employee', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oEmployee) {

                ShiftService.loadEntities(true).then(function(oResponse) {
                    $scope.Shifts = oResponse.Result;
                });

                LevelService.loadEntities(true).then(function(oResponse) {
                    $scope.Areas = oResponse.Result;
                });

                JobPositionService.loadEntities(true).then(function(oResponse) {
                    $scope.JobPositions = oResponse.Result;
                    $scope.onJobPositionChange();
                });

                EmployeeService.getFilteredPage(0, 1, "?JobPositionKey=1").then(function(oResponse) {
                    $scope.Supervisors = oResponse.Result;
                });

                SkillService.loadEntities(true).then(function(oResponse) {
                    $scope.theSkills = oResponse.Result;
                });

                ctrl.load(oEmployee);
            }

            $scope.displayProperty = 'Value';


            $scope.querySkills = function(query, oCurrentItem) {
                return $scope.theSkills ? $scope.theSkills
                    .filter(function(oMember) {
                        if (query) {
                            var lowercaseQuery = angular.lowercase(query || null);
                            var lowercaseItem = angular.lowercase(oMember[$scope.displayProperty]);
                            return lowercaseItem.indexOf(lowercaseQuery) > -1;
                        } else {
                            return [];
                        }
                    })
                    .filter(function(oMember) {
                        return !$scope.baseEntity.SelectedSkills.find(function(oCurrent) {
                            return oCurrent[$scope.displayProperty] == oMember[$scope.displayProperty];
                        });
                    }) : [];
            };

            $scope.onJobPositionChange = function() {
                let oFound = $scope.JobPositions.find(j => j.id == $scope.baseEntity.JobPositionKey);
                if (oFound) {
                    $scope.JobPositionSkills = oFound.Skills;
                } else {
                    $scope.JobPositionSkills = [];
                }
            };

            $scope.isMissingSkill = function(chip) {
                let oFound = $scope.baseEntity.SelectedSkills.find(s => s.Value.toLowerCase() == chip.Value.toLowerCase());
                if (!oFound) {
                    return 'isMissingSkill';
                } else {
                    return '';
                }
            }

            $scope.newSkill = function(chip) {
                var oItem = {};
                if (chip.hasOwnProperty('SkillKey')) {
                    oItem.Value = chip.Value;
                    oItem.SkillKey = chip.SkillKey
                } else {
                    oItem.Value = chip;
                }
                return oItem;
            };

        }
    };
});
