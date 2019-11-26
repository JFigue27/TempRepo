'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Certification
 * @description
 * # Certification
 */
angular.module('Training').directive('certificationForm', function() {
    return {
        templateUrl: 'components/Certification/certification.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, CertificationService, LevelService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Certification',
                baseService: CertificationService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-Certification', function(scope, oCertification) {
                refresh(oCertification);
            });

            $scope.$on('ok-modal-Certification', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oCertification) {
                ctrl.load(oCertification);
            }

            LevelService.loadEntities(true).then(function(oResponse) {
                $scope.theProductionLines = oResponse.Result;
            });

            $scope.displayProperty = 'Value';


            $scope.queryProductionLines = function(query, oCurrentItem) {
                return $scope.theProductionLines ? $scope.theProductionLines
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
                        return !$scope.baseEntity.SelectedProductionLines.find(function(oCurrent) {
                            return oCurrent[$scope.displayProperty] == oMember[$scope.displayProperty];
                        });
                    }) : [];
            };

        }
    };
});
