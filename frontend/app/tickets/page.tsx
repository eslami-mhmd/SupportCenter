"use client";

import { useEffect, useState } from "react";
import { useLanguage } from "@/context/LanguageContext";
import { apiFetch } from "@/lib/api";
import Link from "next/link";
import { Ticket as TicketIcon, Plus, Eye } from "lucide-react";

interface Ticket {
  id: string;
  subject: string;
  description: string;
  status: string;
  priority: string;
  createdAt?: string;
  tenantId?: string;
}

export default function TicketsPage() {
  const { t, dir } = useLanguage();
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    apiFetch<Ticket[]>("/tickets")
      .then((data) => {
        setTickets(Array.isArray(data) ? data : []);
      })
      .catch((err) => {
        setError(err.message || "Failed to load tickets");
      })
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div className="flex items-center space-x-3 rtl:space-x-reverse">
          <div className="p-2 bg-indigo-50 text-indigo-600 rounded-lg">
            <TicketIcon className="h-6 w-6" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900">{t("ticketList")}</h1>
        </div>
        <Link
          href="/tickets/new"
          className="bg-indigo-600 text-white px-4 py-2 rounded-lg font-semibold hover:bg-indigo-700 transition flex items-center space-x-2 rtl:space-x-reverse shadow"
        >
          <Plus className="h-5 w-5" />
          <span>{t("createTicket")}</span>
        </Link>
      </div>

      {error && (
        <div className="p-4 bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
      )}

      {loading ? (
        <div className="text-center py-12 text-gray-500">{t("loading")}</div>
      ) : tickets.length === 0 ? (
        <div className="bg-white p-12 text-center rounded-xl shadow border border-gray-100 text-gray-500">
          {t("noTickets")}
        </div>
      ) : (
        <div className="bg-white shadow rounded-xl overflow-hidden border border-gray-100">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">{t("subject")}</th>
                <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">{t("status")}</th>
                <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">{t("priority")}</th>
                <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">{t("createdAt")}</th>
                <th className="px-6 py-3 text-end text-xs font-medium text-gray-500 uppercase tracking-wider">{t("actions")}</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {tickets.map((ticket) => (
                <tr key={ticket.id} className="hover:bg-gray-50 transition">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{ticket.subject}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className="px-2.5 py-1 text-xs font-semibold rounded-full bg-indigo-50 text-indigo-700">
                      {ticket.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className="px-2.5 py-1 text-xs font-semibold rounded-full bg-amber-50 text-amber-700">
                      {ticket.priority}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {ticket.createdAt ? new Date(ticket.createdAt).toLocaleDateString() : "-"}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-end text-sm font-medium">
                    <Link
                      href={`/tickets/${ticket.id}`}
                      className="text-indigo-600 hover:text-indigo-900 inline-flex items-center space-x-1 rtl:space-x-reverse"
                    >
                      <Eye className="h-4 w-4" />
                      <span>{dir === "rtl" ? "مشاهده" : "View"}</span>
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
