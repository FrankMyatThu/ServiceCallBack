'use strict';

app.controller('rtdbDataController', ['$scope', 'backendHubProxy',
    function ($scope, backendHubProxy) {
        

        
        //#region GetNotification
        $scope.GetNotification = function () {
            var RtdbHub = backendHubProxy(backendHubProxy.defaultServer, 'RtdbHub');
            RtdbHub.on('broadcastRtdbValue', function (data) {
                data.forEach(function (dataItem) {
                    console.log("dataItem.value = ", dataItem.value);
                });
            });
        }
        //#endregion

        

    }
]);
