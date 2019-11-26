'use strict';

/**
 * @ngdoc function
 * @name Training.controller:LevelController
 * @description
 * # LevelController
 * Controller of the Training
 */
angular.module('Training').controller('LevelController', function($scope, formController, $mdDialog, LevelService, CertificationService) {

    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Level',
        baseService: LevelService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });

    $scope.$on('load-modal-Level', function(scope, oLevel) {
        refresh(oLevel);
    });

    $scope.$on('ok-modal-Level', function() {
        $scope.baseEntity.editMode = true;
        return ctrl.save().then(function() {
            $mdDialog.hide();
            alertify.success('Saved Successfully.');
        });
    });

    function refresh(oLevel) {
        ctrl.load(oLevel);
    }

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

});
