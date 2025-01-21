import { useCallback } from "react";
import "./App.css";
import { Outlet } from "react-router";
import { v4 as uuidv4 } from "uuid";

type Observee = {
  id: string;
  callback: any;
};

class Observer {
  public observees: Observee[] = [];

  onChange(callback: any) {
    console.log("before subscribe", this.observees);
    const id = uuidv4();
    this.observees.push({ id: id, callback: callback });
    console.log("after subscribe", this.observees);
    return () => {
      console.log("before cleanup", this.observees);
      this.observees = this.observees.filter((o) => o.id !== id);
      console.log("after cleanup", this.observees);
    };
  }

  trigger() {
    this.observees.forEach((o) => o.callback(10));
  }
}

const observer = new Observer();
setInterval(() => {
  observer.trigger();
}, 1000 / 144);

export function App() {
  const setRef = useCallback((ref: HTMLElement | null) => {
    let width = 100;
    const callback = (increase: number) => {
      if (ref) {
        width += increase;
        ref.style.width = `${width}px`;
      }
    };

    const unsubscribe = observer.onChange(callback);

    return () => {
      unsubscribe();
    };
  }, []);
  return (
    <>
      <div style={{ backgroundColor: "red" }} ref={setRef}>
        Vite + React
      </div>
      <Outlet />
    </>
  );
}
