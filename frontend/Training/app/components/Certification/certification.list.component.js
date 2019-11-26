'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Certification
 * @description
 * # Certification
 */
angular.module('Training').directive('certificationList', function() {
    return {
        templateUrl: 'components/Certification/certification.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, CertificationService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Certification',
                baseService: CertificationService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Certification',
                        contentElement: '#modal-Certification',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Certification', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Certification',
                        contentElement: '#modal-Certification',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Certification', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Certification', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
