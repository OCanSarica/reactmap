import constant from '../constants/authenticationConstant';
import { history } from '../tools';
import { authenticationService } from '../services';
import { toast } from 'react-toastify';

const login = (username, password) => {

    return dispatch => {

        dispatch(request());

        authenticationService.login(username, password).
            then(response => {

                if (!response.success) {

                    toast.error(response.exception);

                    dispatch(fail());

                    return;
                }

                localStorage.setItem("user_", JSON.stringify(response.data));

                localStorage.setItem('token_', response.data.token);

                dispatch(success(response.data));

                history.push("/");
            }).
            catch(ex => {

                console.error(ex);

                toast.error("Giriş yapılamadı!");

                dispatch(fail());
            });
    };

    function success(user) { return { type: constant.LOGIN_SUCCESS, payload: user }; }
    function fail() { return { type: constant.LOGIN_FAIL }; }
    function request() { return { type: constant.LOGIN_REQUEST }; }
}

const logout = () => {

    localStorage.clear("user_");

    return { type: constant.LOG_OUT };
}

export const authenticationAction = {
    login,
    logout
}

export default authenticationAction;