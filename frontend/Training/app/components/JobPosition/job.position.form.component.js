'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:JobPosition
 * @description
 * # JobPosition
 */
angular.module('Training').directive('jobPositionForm', function() {
    return {
        templateUrl: 'components/JobPosition/job.position.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, JobPositionService, SkillService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'JobPosition',
                baseService: JobPositionService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-JobPosition', function(scope, oJobPosition) {
                refresh(oJobPosition);
            });

            $scope.$on('ok-modal-JobPosition', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oJobPosition) {
                ctrl.load(oJobPosition);
            }

            SkillService.loadEntities(true).then(function(oResponse) {
                $scope.theSkills = oResponse.Result;
            });
        
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

        }
    };
});
