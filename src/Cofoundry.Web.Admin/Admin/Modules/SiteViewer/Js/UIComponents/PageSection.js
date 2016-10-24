﻿angular.module('cms.siteViewer').directive('cmsPageSection', [
    '$window',
    '$timeout',
    '_',
    'shared.modalDialogService',
    'siteViewer.modulePath',
function (
    $window,
    $timeout,
    _,
    modalDialogService,
    modulePath
    ) {

    return {
        restrict: 'E',
        templateUrl: modulePath + 'uicomponents/PageSection.html',
        controller: ['$scope', Controller],
        link: link,
        replace: true
    };

    /* CONTROLLER */
    function Controller($scope) {

        this.getSectionParams = function () {

            return _.pick($scope, [
                'siteFrameEl',
                'refreshContent',
                'pageTemplateSectionId',
                'isMultiModule',
                'isCustomEntity'
            ]);
        }
    };

    /* LINK */
    function link(scope, el, attrs) {
        var overTimer;

        init();

        /* Init */
        function init() {

            scope.isOver = false;

            scope.setIsOver = setIsOver;
            scope.addModule = addModule;

            scope.$watch('sectionAnchorElement', onAnchorChanged);
            scope.$watch('isSectionOver', setIsOver);
            scope.$watch('scrolled', onScroll);
        }

        /* UI Actions */
        function addModule() {

            scope.isPopupActive = true;

            modalDialogService.show({
                templateUrl: modulePath + 'routes/modals/addmodule.html',
                controller: 'AddModuleController',
                options: {
                    anchorElement: scope.sectionAnchorElement,
                    pageTemplateSectionId: scope.pageTemplateSectionId,
                    onClose: onClose,
                    refreshContent: refreshSection,
                    isCustomEntity: scope.isCustomEntity,
                }
            });

            function onClose() {
                scope.isPopupActive = false;
            }
        }

        function setIsOver(isOver) {

            if (isOver) {
                if (overTimer) {
                    $timeout.cancel(overTimer);
                    overTimer = null;
                }
                scope.isOver = true;
                setAnchorOver(scope.sectionAnchorElement, true);

            } else if (!overTimer) {

                // a timeout is required to reduce flicker, any timeout value pushes
                // the changes to the next digest cycle, but also i've made it longer
                // than necessary to give a small delay on the hover out.
                overTimer = $timeout(function () {
                    scope.isOver = false;
                    setAnchorOver(scope.sectionAnchorElement, false);
                }, 300);
            }
        }

        /* Events */
        function onAnchorChanged(newAnchorElement, oldAnchorElement) {
            if (newAnchorElement) {

                scope.pageTemplateSectionId = newAnchorElement.attr('data-cms-page-template-section-id');
                scope.sectionName = newAnchorElement.attr('data-cms-page-section-name');
                scope.isMultiModule = newAnchorElement.attr('data-cms-multi-module');
                scope.isCustomEntity = newAnchorElement[0].hasAttribute('data-cms-custom-entity-section');
                setPosition();
            }

            // Remove over css if the overTimer was cancelled
            setAnchorOver(oldAnchorElement, false)

            function setPosition() {
                var siteFrameEl = scope.siteFrameEl,
                    elementOffset = newAnchorElement.offset(),
                    siteFrameOffset = siteFrameEl.offset(),
                    iframeDoc = siteFrameEl[0].contentDocument.documentElement;

                var top = elementOffset.top + siteFrameOffset.top - iframeDoc.scrollTop + 2;

                if (top < siteFrameOffset.top) {
                    top = siteFrameOffset.top;
                }

                // popover hovers in the right hand corner of the element
                var right = -8 + $window.innerWidth - (elementOffset.left + newAnchorElement[0].offsetWidth - iframeDoc.scrollLeft);

                scope.css = {
                    top: top + 'px',
                    right: (right || 0) + 'px'
                };

                scope.startY = top;
            }
        }

        function onScroll(e) {
            var y = scope.startY - e;
            if (y) {
                scope.css = {
                    top: y + 'px',
                    right: scope.css.right
                }
            }
        }

        /* Private Helpers */
        function refreshSection() {

            return scope.refreshContent({
                pageTemplateSectionId: scope.pageTemplateSectionId
            });
        }

        function setAnchorOver(anchorEl, isOver) {
            if (anchorEl) anchorEl.toggleClass('cms-hover-section', isOver);
        }
    }

}]);