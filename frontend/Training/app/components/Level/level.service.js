'use strict';

/**
 * @ngdoc function
 * @name Training.service:LevelService
 * @description
 * # LevelService
 * Service of the Training
 */
angular.module('Training').service('LevelService', function(crudFactory) {

    var crudInstance = new crudFactory({

        entityName: 'Level1',

        catalogs: [],

        adapter: function(theEntity) {

            theEntity.SelectedCertifications = [];
            if (theEntity.Certifications) {
                theEntity.SelectedCertifications = theEntity.Certifications;
            }
            theEntity.sCertifications = theEntity.SelectedCertifications
                .map(function(current) {
                    return '' + current.Value;
                })
                .join('; ');
            if (theEntity.sCertifications == '' || theEntity.sCertifications == '[]') {
                theEntity.sCertifications = '(Empty)';
            }

            return theEntity;
        },

        adapterIn: function(theEntity) {

        },

        adapterOut: function(theEntity, self) {

        },

        dependencies: [

        ]
    });

    crudInstance.LevelName = 'Area';

    return crudInstance;
});
