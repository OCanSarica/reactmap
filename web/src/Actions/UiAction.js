import constant from '../constants/uiConstant';
import tool from "../tools/otherTool";

const openBottomPanel = (children, title) => {

    return async (dispatch) => {

        dispatch(closeBottomPanel());

        await tool.sleep(100);

        dispatch(open(children, title));
    };

    function open(children, title) {

        return { type: constant.OPEN_BOTTOM_PANEL, payload: { children, title } };
    }
}

const minimizeBottomPanel = () => {

    return { type: constant.MINIMIZE_BOTTOM_PANEL };
}

const maximizeBottomPanel = () => {

    return { type: constant.MAXIMIZE_BOTTOM_PANEL };
}

const closeBottomPanel = () => {

    return { type: constant.CLOSE_BOTTOM_PANEL };
}

const openRightPanel = (children, title) => {

    return async (dispatch) => {

        dispatch(closeRightPanel());

        await tool.sleep(100);

        dispatch(open(children, title));
    };

    function open(children, title) {

        return { type: constant.OPEN_RIGHT_PANEL, payload: { children, title } };
    }
}

const closeRightPanel = () => {

    return { type: constant.CLOSE_RIGHT_PANEL };
}

export const uiAction = {
    openBottomPanel,
    closeBottomPanel,
    minimizeBottomPanel,
    maximizeBottomPanel,
    openRightPanel,
    closeRightPanel
}

export default uiAction;
