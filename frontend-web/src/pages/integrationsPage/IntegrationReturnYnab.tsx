import React from "react";
import {generatePath, Navigate, useSearchParams} from "react-router-dom";

import Loader from "../../components/Loader";
import {useYnabReturnService} from "../../data/useYnabReturnService";
import {paths} from "../../paths";
import stringIsEmpty from "../../util/stringIsEmpty";

export const getYnabReturnUrl = () => new URL(
    generatePath(paths.integrations.return.ynab),
    window.location.href
).toString();

const IntegrationReturnYnab = () => {
    const [search] = useSearchParams();
    const returnCode = search.get("code");
    const returnUrl = getYnabReturnUrl();
    const {isLoading} = useYnabReturnService(returnCode, returnUrl);
    
    if (stringIsEmpty(returnCode)) {
        return <Navigate to={paths.errorNotFound} replace={true} />
    }

    if (isLoading) {
        return <Loader />;
    } else {
        return <Navigate to={paths.integrations.index} replace={true} />
    }
};

export default IntegrationReturnYnab;