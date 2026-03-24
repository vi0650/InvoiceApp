import { Observable, of } from 'rxjs';
import { tap, catchError, finalize } from 'rxjs/operators';
import { HotToastService } from '@ngxpert/hot-toast';

export function hotToastObserve<T>(
  toast: HotToastService,
  options: {
    loading: string;
    success: string | ((data: T) => string);
    error: string | ((err: any) => string);
    dismissible?: boolean;
  }
) {
  const loadingToast = toast.loading(options.loading, {
    dismissible: options.dismissible ?? true,
  });

  return (source: Observable<T>) =>
    source.pipe(
      tap((res) => {
        const message =
          typeof options.success === 'function'
            ? options.success(res)
            : options.success;

        if (message && message.trim() !== "") {
          toast.success(message, {
            dismissible: options.dismissible ?? true,
          });
        }
      }),

      catchError((err) => {
        const errorMsg =
          typeof options.error === 'function'
            ? options.error(err)
            : options.error;

        toast.error(errorMsg, { duration: 4000, dismissible: options.dismissible ?? true },);
        return of(null as T);
      }),

      finalize(() => {
        loadingToast.close();
      })
    );
}
