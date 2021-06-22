import constant from '../constants/authenticationConstant';

const user = JSON.parse(localStorage.getItem('user_'));

const initialState = user ? { user } : {};

export function authenticationReducer(state = initialState, action) {

    switch (action.type) {

        case constant.LOGIN_FAIL:
            return { };

        case constant.LOGIN_REQUEST:
            return { loginRequest: true };

        case constant.LOGIN_SUCCESS:
            return { user: action.payload };

        case constant.LOG_OUT:
            return { user: null };

        default:
            return state;
    }
}

export default authenticationReducer;
