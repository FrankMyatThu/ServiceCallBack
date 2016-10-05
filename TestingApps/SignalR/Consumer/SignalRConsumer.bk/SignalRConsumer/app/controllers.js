'use strict';

app.controller('rtdbDataController', ['$scope', 'backendHubProxy',
    function ($scope, backendHubProxy) {
        var RtdbHub = backendHubProxy(backendHubProxy.defaultServer, 'RtdbHub');

        
        //#region GetNotification
        $scope.GetNotification = function () {
            RtdbHub.on('broadcastRtdbValue', function (data) {
                data.forEach(function (dataItem) {
                    console.log("dataItem.value = ", dataItem.value);
                });
            });
        }
        //#endregion

        

    }
]);
