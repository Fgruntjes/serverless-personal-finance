import {renderHook} from "@testing-library/react";

import {useTenant} from "./tenant";

describe(useTenant.name, () => {
    test("current tenant is static", () => {
        const tenant = renderHook(() => useTenant()).result.current.currentTenant;
    
        expect(tenant).not.toBeUndefined();
    });
});