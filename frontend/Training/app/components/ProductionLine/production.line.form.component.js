'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:ProductionLine
 * @description
 * # ProductionLine
 */
angular.module('Training').directive('productionLineForm', function() {
    return {
        templateUrl: 'components/ProductionLine/production.line.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, ProductionLineService, LevelService, CertificationService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'ProductionLine',
                baseService: ProductionLineService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-ProductionLine', function(scope, oProductionLine) {
                refresh(oProductionLine);
            });

            $scope.$on('ok-modal-ProductionLine', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oProductionLine) {
                ctrl.load(oProductionLine);
            }

            LevelService.loadEntities(true).then(function(oResponse) {
                $scope.Areas = oResponse.Result;
            });

            CertificationService.loadEntities(true).then(function(oResponse) {
                $scope.theCertifications = oResponse.Result;
            });

            $scope.displayProperty = 'Value';


            $scope.queryCertifications = function(query, oCurrentItem) {
                return $scope.theCertifications ? $scope.theCertifications
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
                        return !$scope.baseEntity.SelectedCertifications.find(function(oCurrent) {
                            return oCurrent[$scope.displayProperty] == oMember[$scope.displayProperty];
                        });
                    }) : [];
            };


        }
    };
});
