(function () {
    angular.module('app').controller('app.views.manageknowledgecenter.Details', [
        '$scope', '$uibModalInstance', '$stateParams', 'abp.services.app.knowledgeCenter', 'id',
        function ($scope, $uibModalInstance, $stateParams, knowledgeCenterService, item) {
            var vm = this;
            vm.saving = false;
            vm.items = {};
            var init = function () {

                vm.saving = true;
                knowledgeCenterService.getknowledgeCenterForDetail({ id: item })
                    .then(function (result) {
                        vm.items = result.data;
                    });
                getknowledgeCenterDocuments(item);
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            vm.downloadFile = function (filepath) {
                var filepath = "userfiles/KnowledgeCenter/" + filepath;
                window.open(filepath, "_blank");
            };

            function getknowledgeCenterDocuments() {
                $(".ladermain").show();

                knowledgeCenterService.getknowledgeCenterDocuments({ id: item }).then(function (result) {
                    vm.knowledgeCenterAttachments = result.data.items;
                    vm.projectAttachments = result.data.items;
                    $(".ladermain").hide();
                });
            }

            init();
        }
    ]);
})();