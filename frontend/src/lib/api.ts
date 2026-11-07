const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5240/api";

interface FetchOptions extends RequestInit {
  token?: string;
}

export async function apiFetch<T>(endpoint: string, options: FetchOptions = {}): Promise<T> {
  const { token, headers, ...rest } = options;
  
  const authHeaders: Record<string, string> = {};
  const authToken = token || (typeof window !== "undefined" ? localStorage.getItem("auth_token") : null);
  
  if (authToken) {
    authHeaders["Authorization"] = `Bearer ${authToken}`;
  }

  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...rest,
    headers: {
      "Content-Type": "application/json",
      ...authHeaders,
      ...(headers as Record<string, string>),
    },
  });

  if (!res.ok) {
    const errorText = await res.text();
    let errorMessage = `API error: ${res.status} ${res.statusText}`;
    try {
      const errorJson = JSON.parse(errorText);
      errorMessage = errorJson.message || errorJson.title || errorMessage;
    } catch {
      if (errorText) errorMessage = errorText;
    }
    throw new Error(errorMessage);
  }

  if (res.status === 204) {
    return {} as T;
  }

  const data = await res.json();
  return data;
}
