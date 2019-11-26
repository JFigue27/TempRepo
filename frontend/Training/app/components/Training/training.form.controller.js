'use strict';

/**
 * @ngdoc function
 * @name Training.controller:TrainingController
 * @description
 * # TrainingController
 * Controller of the Training
 */
angular.module('Training').controller('TrainingController', function($scope, formController, TrainingService, $mdDialog) {

    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Training',
        baseService: TrainingService,
        afterCreate: function(oEntity) {
            $scope.baseEntity.TrainingScores = [];
            $scope.baseEntity.TrainingScores.push({
                Employee: employee
            });
            $scope.baseEntity.CertificationKey = $scope.theCertification.id;
            $scope.baseEntity.CatCertification = $scope.theCertification;
            $scope.baseEntity.ConvertedDateCertification = getNow();
            $scope.onDateCertificationChanges();
            $scope.baseEntity.Level1Key = employee.Level1Key;
            $scope.baseEntity.TrainingScores[0].Score = 100;
            $scope.baseEntity.QuickTraining = true;
        },
        afterLoad: function(oEntity) {

            if ($scope.baseEntity.TrainingScores && $scope.baseEntity.TrainingScores.length > 0) {
                $scope.baseEntity.TrainingScores[0].Employee = employee;
                $scope.baseEntity.CatCertification = $scope.theCertification;
            }

        }
    });


    var employee;
    $scope.$on('initializeQuickTraining', function(scope, oCertification, oEmployee) {
        loadCertification(oCertification, oEmployee);
    });

    function loadCertification(oCertification, oEmployee) {
        employee = oEmployee;
        $scope.theCertification = oCertification;
        ctrl.load(oCertification.TrainingInfo);
    }


    $scope.$on('ok-modal-QuickCertification', function() {
        $scope.baseEntity.editMode = true;
        return ctrl.save().then(function() {
            $mdDialog.hide();
            alertify.success('Saved Successfully.');
        });
    });

    $scope.$on('remove-modal-QuickCertification', function() {
        return $scope.remove($scope.baseEntity).then(function() {
            $mdDialog.hide();
            alertify.success('Remove Successfully.');
        });
    });

    $scope.$on('new-click-modal-QuickCertification', function() {
        $scope.theCertification.TrainingInfo = null;
        loadCertification($scope.theCertification, employee);
    });

    function getNow() {
        var d = new Date();
        d.setSeconds(0, 0);
        return d;
    };

    $scope.onDateCertificationChanges = function() {
        var iLifecycleInMonths = $scope.theCertification.LifecycleInMonths;
        var dExpiresAt = moment($scope.baseEntity.ConvertedDateCertification).add(iLifecycleInMonths, 'months');
        $scope.baseEntity.ConvertedDateExpiresAt = dExpiresAt.toDate();
    };

}).directive('quickTraining', function() {
    return {
        templateUrl: 'components/Training/QuickTraining.html',
        restrict: 'E',
        scope: {

        },
        controller: 'TrainingController'
    };
});
