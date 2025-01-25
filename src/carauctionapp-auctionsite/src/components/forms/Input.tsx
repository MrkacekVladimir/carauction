import { InputHTMLAttributes, Ref } from "react";
import classes from "./Input.module.css";

interface Props extends InputHTMLAttributes<HTMLInputElement> {
  ref?: Ref<HTMLInputElement>;
}

export const Input = (props: Props) => {
  return <input className={classes.input} {...props} />;
};
