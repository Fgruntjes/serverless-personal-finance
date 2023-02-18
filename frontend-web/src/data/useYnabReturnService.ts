import {useQuery, useQueryClient} from "@tanstack/react-query";
import {useTranslation} from "react-i18next";
import {toast} from "react-toastify";

import {ReturnService, StatusService} from "../generated/App.Function.Integration.Ynab";
import {TranslationNamespaces} from "../locales/namespaces";
import {bindPromiseToSignal} from "../util/bindPromiseToSignal";
import stringIsEmpty from "../util/stringIsEmpty";
import useErrorToString from "../util/useErrorToString";

export function useYnabReturnService(returnCode: string|null, returnUrl: string) {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);
    const errorToString = useErrorToString();
    const queryClient = useQueryClient();

    return useQuery(
        {
            retry: false,
            queryKey: [ReturnService.name, {returnCode, returnUrl}],
            queryFn: ({signal}) => {
                if (!stringIsEmpty(returnCode)) {
                    return bindPromiseToSignal(ReturnService.return(returnCode as string, returnUrl), signal);
                }
                
                throw new Error(t("error.missingReturnCode"));
            },
            onError: (error) => {
                toast.error(t("error.processReturnCodeFailed", {error: errorToString(error)}));
            },
            onSuccess: () => {
                toast.success(t("processReturnCodeSuccess"));
            },
            onSettled: () => queryClient.invalidateQueries({queryKey: [StatusService.name]})
        }
    );
}