import { InterfaceA } from "./InterfaceA";
import { InterfaceB } from "./InterfaceB";

export interface ClassA extends InterfaceA, InterfaceB {
    ClassProp: number;
}