'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:modal
 * @description
 * # modal
 */
angular.module('Training').directive('modal', function() {
    return {
        template: `<div style="display: none;">
                        <div class="md-dialog-container" id="{{modalId}}">
                            <md-dialog aria-label="Dialog" flex="theFlex" layout="column" style="max-height: 100%;" ng-style="{'max-width': maxWidth, height: theHeight}">
                                <md-toolbar ng-if="title">
                                  <div class="md-toolbar-tools">
                                    <h2>{{title}}</h2>
                                  </div>
                                </md-toolbar>
                                <div ng-transclude="header"></div>
                                <md-dialog-content flex>
                                    <div class="md-dialog-content" flex>
                                        <div ng-transclude="body" flex></div>
                                    </div>
                                </md-dialog-content>
                                <md-dialog-actions layout="row" layout-align="end end" style="border-top: 1px solid rgba(0,0,0,0.12);">
                                    <md-button ng-if="removeLabel" class="md-warn" ng-click="remove_click();">
                                        <md-icon>delete</md-icon>{{removeLabel}}
                                    </md-button>
                                    <div flex></div>

                                    <md-button ng-if="newClick" ng-click="new_click();">
                                        <md-icon>autorenew</md-icon>New
                                    </md-button>
                                    <md-button ng-if="showPrint" ng-click="print_modal();">
                                        <md-icon>print</md-icon>Print
                                    </md-button>
                                    <md-button ng-click="close_modal()" class="">Close</md-button>
                                    <md-button ng-if="okLabel" ng-click="ok_click()" class="">{{okLabel}}</md-button>
                                </md-dialog-actions>
                            </md-dialog>
                        </div>
                    </div>`,
        restrict: 'E',
        transclude: {
            header: '?modalHeader',
            body: 'modalBody'
        },
        replace: false,
        scope: {
            modalId: '@',
            okLabel: '@',
            maxWidth: '=',
            title: '@',
            fullScreen: '=',
            showPrint: '@',
            newClick: '@',
            removeLabel: '@'
        },
        controller: function($scope, $mdDialog) {
            $scope.baseEntity = {};

            $scope.ok_click = function() {
                $scope.$broadcast('ok-' + $scope.modalId);
            };

            $scope.close_modal = function() {
                $mdDialog.hide();
                $scope.$broadcast('unload-' + $scope.modalId);
            };

            $scope.print_modal = function() {
                $scope.$broadcast('print-' + $scope.modalId);
            };

            $scope.new_click = function() {
                $scope.$broadcast('new-click-' + $scope.modalId);
            };

            $scope.remove_click = function() {
                $scope.$broadcast('remove-' + $scope.modalId);
            };

            $scope.theFlex = 95;
            $scope.theHeight = '95%';
            if ($scope.fullScreen) {
                $scope.theFlex = 100;
                $scope.theHeight = '100%';
            }
        }
    };
});
