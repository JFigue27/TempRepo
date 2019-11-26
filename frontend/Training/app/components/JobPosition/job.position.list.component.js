'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:JobPosition
 * @description
 * # JobPosition
 */
angular.module('Training').directive('jobPositionList', function() {
    return {
        templateUrl: 'components/JobPosition/job.position.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, JobPositionService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'JobPosition',
                baseService: JobPositionService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'JobPosition',
                        contentElement: '#modal-JobPosition',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-JobPosition', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'JobPosition',
                        contentElement: '#modal-JobPosition',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-JobPosition', oEntity);
                },
                filters: []
            });

            $scope.$on('load_JobPosition', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
