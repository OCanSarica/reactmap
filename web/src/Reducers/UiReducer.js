import constant from '../constants/uiConstant';

const initialState = {
    bottomPanel: {
        visible: false,
        children: null,
        title: null,
        minimize: false
    },
    rightPanel: {
        visible: false,
        children: null,
        title: null
    }
}
export function uiReducer(state = initialState, action) {

    switch (action.type) {

        case constant.OPEN_BOTTOM_PANEL:
            return {
                ...state,
                bottomPanel: {
                    visible: true,
                    children: action.payload.children,
                    title: action.payload.title,
                    minimize: false
                }
            };

        case constant.CLOSE_BOTTOM_PANEL:
            return {
                ...state,
                bottomPanel: initialState.bottomPanel
            };

        case constant.MAXIMIZE_BOTTOM_PANEL:
            return {
                ...state,
                bottomPanel: {
                    ...state.bottomPanel,
                    minimize: false
                }
            };

        case constant.MINIMIZE_BOTTOM_PANEL:
            return {
                ...state,
                bottomPanel: {
                    ...state.bottomPanel,
                    minimize: true
                }
            };

        case constant.OPEN_RIGHT_PANEL:
            return {
                ...state,
                rightPanel: {
                    visible: true,
                    children: action.payload.children,
                    title: action.payload.title
                }
            };

        case constant.CLOSE_RIGHT_PANEL:
            return {
                ...state,
                rightPanel: initialState.rightPanel
            };

        default:
            return state;
    }
}

export default uiReducer;

