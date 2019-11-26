'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Skill
 * @description
 * # Skill
 */
angular.module('Training').directive('skillList', function() {
    return {
        templateUrl: 'components/Skill/skill.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, SkillService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Skill',
                baseService: SkillService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Skill',
                        contentElement: '#modal-Skill',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Skill', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Skill',
                        contentElement: '#modal-Skill',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Skill', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Skill', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
