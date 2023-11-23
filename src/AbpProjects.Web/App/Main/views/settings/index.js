(function () {
    angular.module('app').controller('app.views.settings.index', [
        '$scope', 'appSession', 'abp.services.app.hostSetting', 'abp.services.app.commonLookup',
        function ($scope, appSession, hostSettingService, commonLookupService) {

            var vm = this;
            var usingDefaultTimeZone = false;
            var initialTimeZone = null;
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.loading = false;
            vm.settings = null;
            vm.editions = [];
            vm.testEmailAddress = appSession.user.emailAddress;
            vm.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;

            vm.getSettings = function () {
                vm.loading = true;
                hostSettingService.getAllSettings()
                    .then(function (result) {
                        vm.settings = result.data;
                        initialTimeZone = vm.settings.general.timezone;
                        usingDefaultTimeZone = vm.settings.general.timezoneForComparison === abp.setting.values["Abp.Timing.TimeZone"];
                    }).finally(function () {
                        vm.loading = false;
                    });
            };

            vm.saveAll = function () {
                if (vm.settings.tenantManagement.defaultEditionId === "null") {
                    vm.settings.tenantManagement.defaultEditionId = null;
                }

                //console.log('vm.settings', vm.settings);

                hostSettingService.updateAllSettings(
                  vm.settings
                ).then(function () {
                    abp.notify.success(App.localize('SavedSuccessfully'));

                    if (abp.clock.provider.supportsMultipleTimezone && usingDefaultTimeZone && initialTimeZone !== vm.settings.general.timezone) {
                        abp.message.info(App.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function () {
                            window.location.reload();
                        });
                    }
                });
            };

            vm.sendTestEmail = function () {
                hostSettingService.sendTestEmail({
                    emailAddress: vm.testEmailAddress
                }).then(function () {
                    abp.notify.success(App.localize('TestEmailSentSuccessfully'));
                });
            };

            vm.getEditionValue = function (item) {
                if (item.value) {
                    return parseInt(item.value);
                }
                return item.value;
            };

            vm.getEditions = function () {
                commonLookupService.getEditionsForCombobox({}).then(function (result) {
                    vm.editions = result.data.items;
                    vm.editions.unshift({ value: null, displayText: App.localize('NotAssigned') });
                });
            };

            //function getAbpSettingsList() {
            //    commonLookupService.getAbpSettingsList()
            //        .then(function (result) {
            //            vm.AbpSettingslist = result.data;
            //            vm.settings.cdnDoc = result.data.cdnDoc;
            //        });
            //}

            init = function () {
                vm.getEditions();
                vm.getSettings();
                //getAbpSettingsList();
            }

            init();
        }
    ]);



})();