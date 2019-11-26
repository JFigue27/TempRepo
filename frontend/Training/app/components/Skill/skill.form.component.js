'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Skill
 * @description
 * # Skill
 */
angular.module('Training').directive('skillForm', function() {
    return {
        templateUrl: 'components/Skill/skill.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, SkillService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Skill',
                baseService: SkillService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-Skill', function(scope, oSkill) {
                refresh(oSkill);
            });

            $scope.$on('ok-modal-Skill', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oSkill) {
                ctrl.load(oSkill);
            }

        }
    };
});
