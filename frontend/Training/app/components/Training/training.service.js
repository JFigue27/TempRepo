'use strict';

/**
 * @ngdoc function
 * @name Training.service:TrainingService
 * @description
 * # TrainingService
 * Service of the Training
 */
angular.module('Training').service('TrainingService', function(crudFactory, utilsService) {

    var crudInstance = new crudFactory({

        entityName: 'Training',

        catalogs: [],

        adapter: function(theEntity) {

            if (theEntity.TrainingScores) {
                theEntity.TrainingScores.forEach(function(oScore) {

                    if (oScore.Score > 0) {
                        oScore.CssStatusClass = 'CertificationOK';

                        if (theEntity.AboutToExpire) {
                            oScore.CssStatusClass = 'CertificationAboutToExpire';
                        }
                        if (theEntity.IsExpired) {
                            oScore.CssStatusClass = 'CertificationExpired';
                        }
                    }
                    else {
                        console.log('here')
                    }

                });
            }

            theEntity.ConvertedDateCertification = utilsService.toJavascriptDate(theEntity.DateCertification);
            theEntity.ConvertedDateExpiresAt = utilsService.toJavascriptDate(theEntity.DateExpiresAt);
            theEntity.ConvertedDateProgrammed = utilsService.toJavascriptDate(theEntity.DateProgrammed);
            theEntity.ConvertedDateStart = utilsService.toJavascriptDate(theEntity.DateStart);
            theEntity.ConvertedDateEnd = utilsService.toJavascriptDate(theEntity.DateEnd);

            return theEntity;
        },

        adapterIn: function(theEntity) {

        },

        adapterOut: function(theEntity, self) {
            theEntity.DateExpiresAt = utilsService.toServerDate(theEntity.ConvertedDateExpiresAt);
            theEntity.DateCertification = utilsService.toServerDate(theEntity.ConvertedDateCertification);
            theEntity.DateProgrammed = utilsService.toServerDate(theEntity.ConvertedDateProgrammed);
            theEntity.DateStart = utilsService.toServerDate(theEntity.ConvertedDateStart);
            theEntity.DateEnd = utilsService.toServerDate(theEntity.ConvertedDateEnd);
        },

        dependencies: [

        ]
    });

    crudInstance.GetActiveTrainings = function(iLevel1Key) {
        return crudInstance.customGet('GetActiveTrainings/' + iLevel1Key).then(function(data) {
            if (data) {
                data.forEach(function(oCurrent) {
                    crudInstance.adapt(oCurrent);
                })
            }

            return data;
        });
    };

    return crudInstance;
});
